import { useEffect, useState } from "react";
import { StudentService } from "../../services/StudentService";
import { GradeService } from "../../services/GradeService";
import { CourseService } from "../../services/CourseService";
import "../../styles/StudentDashboard.css";

const StudentDashboard = () => {
  const [student, setStudent] = useState(null);
  const [grades, setGrades] = useState([]);
  const [courses, setCourses] = useState([]);

  const loadProfile = async () => {
    const res = await StudentService.getOwn();
    setStudent(res.data);
  };

  const loadGrades = async () => {
    const res = await GradeService.getMine();
    setGrades(res.data);
  };

  const loadCourses = async () => {
    const res = await CourseService.getAll();
    setCourses(res.data);
  };

  useEffect(() => {
    loadProfile();
    loadGrades();
    loadCourses();
  }, []);

  const renderScore = (score) => {
    if (score == null) return "-";
    if (score >= 90) return <span className="score-badge score-high">{score}</span>;
    if (score >= 70) return <span className="score-badge score-mid">{score}</span>;
    if (score >= 50) return <span className="score-badge score-low">{score}</span>;
    return <span className="score-badge score-fail">{score}</span>;
  };

  return (
    <div className="student-dashboard">
      <h2>Student Dashboard</h2>

      {/* Profil Kartı */}
      {student && (
        <div className="profile-card">
          <h3>My Profile</h3>
          <p><strong>Name:</strong> {student.name} {student.surname}</p>
          <p><strong>Number:</strong> {student.number}</p>
        </div>
      )}

      {/* Dersler ve Notlar */}
      <div className="grades-section">
        <h3>My Grades</h3>
        <table className="grades-table">
          <thead>
            <tr>
              <th>Course</th>
              <th>Teacher</th>
              <th>Status</th>
              <th>Score</th>
            </tr>
          </thead>
          <tbody>
            {grades.length > 0 ? (
              grades.map((g) => (
                <tr key={g.id}>
                  <td>{g.courseName}</td>
                  <td>{g.teacherName}</td>
                  <td>{g.courseStatus}</td>
                  <td>{renderScore(g.score)}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="4">No grades available yet</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default StudentDashboard;
