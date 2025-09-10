import { useEffect } from "react";
import { Link } from "react-router-dom";
import "../../styles/AdminDashboard.css";

const AdminDashboard = () => {
  
  useEffect(() => {
    document.body.className = "admin-dashboard-page";
    return () => {
      document.body.className = "";
    };
  }, []);

  return (
    <div className="admin-dashboard">
      <h2>Admin Dashboard</h2>
      <nav>
        <ul>
          <li>
            <Link to="/students">Manage Students</Link>
          </li>
          <li>
            <Link to="/teachers">Manage Teachers</Link>
          </li>
          <li>
            <Link to="/courses">Manage Courses</Link>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default AdminDashboard;
