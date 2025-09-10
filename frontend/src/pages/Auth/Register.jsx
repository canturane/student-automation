import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import api from "../../services/api";
import "../../styles/Register.css";

const Register = ({ onRegister }) => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    email: "",
    password: "",
    role: "Student",
    name: "",
    surname: "",
    numberOrTitle: "",
  });
  const [error, setError] = useState("");


  useEffect(() => {
    document.body.className = "register-page";
    return () => {
      document.body.className = "";
    };
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const res = await api.post("/Auth/register", form);
      const role = res.data.role.toLowerCase();

      localStorage.setItem("token", res.data.token);
      localStorage.setItem("role", role);

      if (onRegister) onRegister(role);

      navigate("/dashboard");
    } catch (err) {
      setError("Kayıt başarısız. Email zaten kullanılıyor olabilir.");
    }
  };

  return (
    <div className="register-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="email"
          placeholder="Email"
          value={form.email}
          onChange={(e) => setForm({ ...form, email: e.target.value })}
        />
        <input
          type="password"
          placeholder="Password"
          value={form.password}
          onChange={(e) => setForm({ ...form, password: e.target.value })}
        />

        {/* Role seçimi */}
        <select
          value={form.role}
          onChange={(e) => setForm({ ...form, role: e.target.value })}
        >
          <option value="Admin">Admin</option>
          <option value="Teacher">Teacher</option>
          <option value="Student">Student</option>
        </select>

        <input
          type="text"
          placeholder="Name"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Surname"
          value={form.surname}
          onChange={(e) => setForm({ ...form, surname: e.target.value })}
        />

        {form.role === "Student" && (
          <input
            type="text"
            placeholder="Number (Student)"
            value={form.numberOrTitle}
            onChange={(e) =>
              setForm({ ...form, numberOrTitle: e.target.value })
            }
          />
        )}

        {form.role === "Teacher" && (
          <input
            type="text"
            placeholder="Title (Teacher)"
            value={form.numberOrTitle}
            onChange={(e) =>
              setForm({ ...form, numberOrTitle: e.target.value })
            }
          />
        )}

        <button type="submit">Kayıt Ol</button>
      </form>

      {error && <p className="register-error">{error}</p>}

      <p>
        Zaten hesabın var mı?{" "}
        <Link to="/login">
          <button type="button">Giriş Yap</button>
        </Link>
      </p>
    </div>
  );
};

export default Register;
