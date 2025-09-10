import { Routes, Route, Navigate } from "react-router-dom";
import { useEffect, useState } from "react";

// Auth
import Login from "./pages/Auth/Login";
import Register from "./pages/Auth/Register";

// Dashboards
import AdminDashboard from "./pages/Dashboard/AdminDashboard";
import TeacherDashboard from "./pages/Dashboard/TeacherDashboard";
import StudentDashboard from "./pages/Dashboard/StudentDashboard";

// Admin CRUD
import Students from "./pages/CRUD/Students";
import Teachers from "./pages/CRUD/Teachers";
import Courses from "./pages/CRUD/Courses";

// Teacher Pages
import MyCourses from "./pages/Teacher/MyCourses";
import GradeManagement from "./pages/Teacher/GradeManagement";

function App() {
  const [role, setRole] = useState(null);

  useEffect(() => {
    const savedRole = localStorage.getItem("role");
    if (savedRole) {
      setRole(savedRole.toLowerCase());
    }
  }, []);

  return (
    <Routes>
      {/* Root → Login */}
      <Route path="/" element={<Navigate to="/login" />} />

      {/* Auth */}
      <Route path="/login" element={<Login onLogin={setRole} />} />
      <Route path="/register" element={<Register onRegister={setRole} />} />

      {/* Dashboard yönlendirmesi */}
      <Route
        path="/dashboard"
        element={
          role === "admin" ? (
            <AdminDashboard />
          ) : role === "teacher" ? (
            <TeacherDashboard />
          ) : role === "student" ? (
            <StudentDashboard />
          ) : (
            <Navigate to="/login" />
          )
        }
      />

      {/* Admin CRUD sayfaları */}
      {role === "admin" && (
        <>
          <Route path="/students" element={<Students />} />
          <Route path="/teachers" element={<Teachers />} />
          <Route path="/courses" element={<Courses />} />
        </>
      )}

      {/* Teacher sayfaları */}
      {role === "teacher" && (
        <>
          <Route path="/my-courses" element={<MyCourses />} />
          <Route path="/grades" element={<GradeManagement />} />
        </>
      )}

      {/* Varsayılan */}
      <Route path="*" element={<Navigate to="/login" />} />
    </Routes>
  );
}

export default App;
