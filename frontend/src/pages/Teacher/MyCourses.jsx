import React, { useEffect, useState } from "react";
import { CourseService } from "../../services/CourseService";
import { StudentService } from "../../services/StudentService";
import "../../styles/MyCourses.css";

const MyCourses = () => {
  const [courses, setCourses] = useState([]);
  const [editedStatus, setEditedStatus] = useState({});
  const [studentsByCourse, setStudentsByCourse] = useState({});
  const [allStudents, setAllStudents] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState({});

  const loadCourses = async () => {
    const res = await CourseService.getMine();
    setCourses(res.data);

    const defaults = {};
    res.data.forEach((c) => (defaults[c.id] = c.status));
    setEditedStatus(defaults);

    for (const course of res.data) {
      const sres = await CourseService.getStudents(course.id);
      setStudentsByCourse((prev) => ({ ...prev, [course.id]: sres.data }));
    }
  };

  const loadAllStudents = async () => {
    const res = await StudentService.getAll();
    setAllStudents(res.data);
  };

  const handleSave = async (id) => {
    const statusMap = {
      Planned: 0,
      Started: 1,
      Finished: 2,
    };

    await CourseService.updateStatus(id, { status: statusMap[editedStatus[id]] });
    loadCourses();
  };

  const handleAddStudent = async (courseId) => {
    if (!selectedStudent[courseId]) return;
    await CourseService.addStudent(courseId, parseInt(selectedStudent[courseId]));
    setSelectedStudent((prev) => ({ ...prev, [courseId]: "" }));
    loadCourses();
  };

  const handleRemoveStudent = async (courseId, studentId) => {
    await CourseService.removeStudent(courseId, studentId);
    loadCourses();
  };

  useEffect(() => {
    loadCourses();
    loadAllStudents();
  }, []);

  return (
    <div className="my-courses-page">
      <h2>My Courses</h2>
      <table className="courses-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Course</th>
            <th>Status</th>
            <th>Teacher</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {courses.map((c) => (
            <React.Fragment key={c.id}>
              <tr>
                <td>{c.id}</td>
                <td>{c.name}</td>
                <td>
                  <select
                    value={editedStatus[c.id] || c.status}
                    onChange={(e) =>
                      setEditedStatus({
                        ...editedStatus,
                        [c.id]: e.target.value,
                      })
                    }
                  >
                    <option value="Planned">Planned</option>
                    <option value="Started">Started</option>
                    <option value="Finished">Finished</option>
                  </select>
                </td>
                <td>{c.teacherName}</td>
                <td>
                  <button onClick={() => handleSave(c.id)}>Kaydet</button>
                </td>
              </tr>

              {/* 🔽 Kursun altına öğrenciler */}
              <tr>
                <td colSpan="5">
                  <div className="enrolled-students">
                    <strong>Enrolled Students:</strong>
                    <ul>
                      {studentsByCourse[c.id] && studentsByCourse[c.id].length > 0 ? (
                        studentsByCourse[c.id].map((s) => (
                          <li key={s.enrollmentId}>
                            {s.name} {s.surname} ({s.number})
                            <button onClick={() => handleRemoveStudent(c.id, s.studentId)}>
                              ❌ Remove
                            </button>
                          </li>
                        ))
                      ) : (
                        <li>No students enrolled yet</li>
                      )}
                    </ul>

                    {/* Öğrenci ekleme */}
                    <div className="add-student">
                      <select
                        value={selectedStudent[c.id] || ""}
                        onChange={(e) =>
                          setSelectedStudent({ ...selectedStudent, [c.id]: e.target.value })
                        }
                      >
                        <option value="">-- Select Student --</option>
                        {allStudents.map((s) => (
                          <option key={s.id} value={s.id}>
                            {s.name} {s.surname} ({s.number})
                          </option>
                        ))}
                      </select>
                      <button onClick={() => handleAddStudent(c.id)}>➕ Add</button>
                    </div>
                  </div>
                </td>
              </tr>
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default MyCourses;
