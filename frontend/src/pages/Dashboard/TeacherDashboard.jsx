import { useEffect } from "react";
import { Link } from "react-router-dom";
import "../../styles/TeacherDashboard.css";

const TeacherDashboard = () => {
  useEffect(() => {
    document.body.className = "teacher-dashboard-page";
    return () => {
      document.body.className = "";
    };
  }, []);

  return (
    <div className="teacher-dashboard">
      <h2>Teacher Dashboard</h2>
      <nav>
        <ul>
          <li>
            <Link to="/my-courses">My Courses</Link>
          </li>
          <li>
            <Link to="/grades">Grade Management</Link>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default TeacherDashboard;
