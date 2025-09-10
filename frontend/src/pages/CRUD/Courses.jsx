import { useEffect, useState } from "react";
import { CourseService } from "../../services/CourseService";
import { TeacherService } from "../../services/TeacherService";
import "../../styles/Courses.css";

const Courses = () => {
  const role = localStorage.getItem("role"); // admin / teacher
  const [courses, setCourses] = useState([]);
  const [teachers, setTeachers] = useState([]);
  const [newCourse, setNewCourse] = useState({ name: "", teacherId: "" });

  const loadCourses = async () => {
    if (role === "admin") {
      const res = await CourseService.getAll();
      setCourses(res.data);
    } else if (role === "teacher") {
      const res = await CourseService.getMine();
      setCourses(res.data);
    }
  };

  const loadTeachers = async () => {
    if (role === "admin") {
      const res = await TeacherService.getAll();
      setTeachers(res.data);
    }
  };

  const handleCreate = async () => {
    if (!newCourse.teacherId) {
      alert("Please select a teacher");
      return;
    }
    await CourseService.create({
      name: newCourse.name,
      teacherId: parseInt(newCourse.teacherId),
    });
    setNewCourse({ name: "", teacherId: "" });
    loadCourses();
  };

  const handleStatusChange = async (courseId, newStatus) => {
    await CourseService.updateStatus(courseId, { status: newStatus });
    loadCourses();
  };

  useEffect(() => {
    loadCourses();
    loadTeachers();
  }, []);

  return (
    <div className="courses-page">
      <h2>Courses</h2>

      {/* Ders Listesi */}
      <table className="courses-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Course Name</th>
            <th>Status</th>
            <th>Teacher</th>
            {role === "teacher" && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {courses.map((c) => (
            <tr key={c.id}>
              <td>{c.id}</td>
              <td>{c.name}</td>
              <td>{c.status}</td>
              <td>{c.teacherName}</td>
              {role === "teacher" && (
                <td>
                  <select
                    value={c.status}
                    onChange={(e) => handleStatusChange(c.id, e.target.value)}
                  >
                    <option value="Planned">Planned</option>
                    <option value="Active">Active</option>
                    <option value="Finished">Finished</option>
                  </select>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>

      {/* Ders Ekleme (sadece Admin) */}
      {role === "admin" && (
        <div className="create-course">
          <h3>Create Course</h3>
          <input
            type="text"
            placeholder="Course Name"
            value={newCourse.name}
            onChange={(e) =>
              setNewCourse({ ...newCourse, name: e.target.value })
            }
          />
          <select
            value={newCourse.teacherId}
            onChange={(e) =>
              setNewCourse({ ...newCourse, teacherId: e.target.value })
            }
          >
            <option value="">-- Select Teacher --</option>
            {teachers.map((t) => (
              <option key={t.id} value={t.id}>
                {t.name} {t.surname} ({t.title})
              </option>
            ))}
          </select>
          <button onClick={handleCreate}>Create</button>
        </div>
      )}
    </div>
  );
};

export default Courses;
