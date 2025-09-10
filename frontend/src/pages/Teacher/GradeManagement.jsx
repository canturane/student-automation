import { useEffect, useState } from "react";
import { CourseService } from "../../services/CourseService";
import { GradeService } from "../../services/GradeService";
import "../../styles/GradeManagement.css";

const GradeManagement = () => {
  const [courses, setCourses] = useState([]);
  const [studentsByCourse, setStudentsByCourse] = useState({});
  const [grades, setGrades] = useState({});
  const [newScores, setNewScores] = useState({});

  const loadCourses = async () => {
    const res = await CourseService.getMine();
    setCourses(res.data);

    for (const course of res.data) {
      const sres = await CourseService.getStudents(course.id);

      setStudentsByCourse((prev) => ({ ...prev, [course.id]: sres.data }));

      const gradeMap = {};
      sres.data.forEach((s) => {
        if (s.score !== null && s.score !== undefined) {
          gradeMap[s.enrollmentId] = s.score;
        }
      });
      setGrades((prev) => ({ ...prev, ...gradeMap }));
    }
  };

  const handleAddGrade = async (enrollmentId, score) => {
    if (!score) return alert("Please enter a score!");
    try {
      await GradeService.add({
        enrollmentId,
        score: parseInt(score),
      });
      alert("Grade added!");
      setGrades((prev) => ({ ...prev, [enrollmentId]: score }));
      setNewScores((prev) => ({ ...prev, [enrollmentId]: "" }));
    } catch (err) {
      console.error("Grade error:", err);
      const message =
        err.response?.data?.message || err.response?.data || err.message;
      alert("Error adding grade: " + message);
    }
  };

  useEffect(() => {
    loadCourses();
  }, []);

  return (
    <div className="grade-page">
      <h2>Grade Management</h2>
      {courses.map((c) => (
        <div key={c.id} className="course-card">
          <h3>
            {c.name} <small>({c.status})</small>
          </h3>
          <table className="grade-table">
            <thead>
              <tr>
                <th>Student</th>
                <th>Number</th>
                <th>Current Grade</th>
                <th>New Grade</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {studentsByCourse[c.id] && studentsByCourse[c.id].length > 0 ? (
                studentsByCourse[c.id].map((s) => (
                  <tr key={s.enrollmentId}>
                    <td>
                      {s.name} {s.surname}
                    </td>
                    <td>{s.number}</td>
                    <td>{grades[s.enrollmentId] ?? "-"}</td>
                    <td>
                      <input
                        type="number"
                        placeholder="0-100"
                        value={newScores[s.enrollmentId] || ""}
                        onChange={(e) =>
                          setNewScores({
                            ...newScores,
                            [s.enrollmentId]: e.target.value,
                          })
                        }
                      />
                    </td>
                    <td>
                      <button
                        onClick={() =>
                          handleAddGrade(
                            s.enrollmentId,
                            newScores[s.enrollmentId]
                          )
                        }
                      >
                        Save
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5">No students enrolled yet</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      ))}
    </div>
  );
};

export default GradeManagement;
