import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import api from "../../services/api";
import "../../styles/Login.css";

const Login = ({ onLogin }) => {
  const navigate = useNavigate();
  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState("");


  useEffect(() => {
    document.body.className = "login-page"; 
    return () => {
      document.body.className = "";
    };
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const res = await api.post("/Auth/login", form);
      const role = res.data.role.toLowerCase();

      // Token ve role kaydet
      localStorage.setItem("token", res.data.token);
      localStorage.setItem("role", role);

      if (onLogin) onLogin(role); // App.jsx state güncelle

      // Dashboard'a yönlendir
      navigate("/dashboard");
    } catch (err) {
      setError("Giriş başarısız. Email veya şifre yanlış.");
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
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
        <button type="submit">Giriş Yap</button>
      </form>

      {error && <p className="error-message">{error}</p>}

      <p>
        Hesabın yok mu?{" "}
        <Link to="/register">
          <button type="button" className="link-button">
            Kayıt Ol
          </button>
        </Link>
      </p>
    </div>
  );
};

export default Login;
