import { useEffect, useState } from "react";
import { TeacherService } from "../../services/TeacherService";
import "../../styles/Teachers.css";

const Teachers = () => {
  const [teachers, setTeachers] = useState([]);
  const [newTeacher, setNewTeacher] = useState({
    email: "",
    password: "",
    name: "",
    surname: "",
    title: "",
  });
  const [editing, setEditing] = useState(null);
  const [editForm, setEditForm] = useState({
    name: "",
    surname: "",
    title: "",
  });

  const loadTeachers = async () => {
    const res = await TeacherService.getAll();
    setTeachers(res.data);
  };

  const handleCreate = async () => {
    await TeacherService.create(newTeacher);
    setNewTeacher({ email: "", password: "", name: "", surname: "", title: "" });
    loadTeachers();
  };

  const handleEdit = (teacher) => {
    setEditing(teacher.id);
    setEditForm({
      name: teacher.name,
      surname: teacher.surname,
      title: teacher.title,
    });
  };

  const handleUpdate = async (id) => {
    await TeacherService.update(id, editForm);
    setEditing(null);
    loadTeachers();
  };

  useEffect(() => {
    loadTeachers();
  }, []);

  return (
    <div className="teachers-page">
      <h2>Teachers</h2>

      {/* Öğretmen Listesi */}
      <div className="teachers-list">
        <ul>
          {teachers.map((t) => (
            <li key={t.id}>
              {editing === t.id ? (
                <>
                  <input
                    type="text"
                    value={editForm.name}
                    onChange={(e) =>
                      setEditForm({ ...editForm, name: e.target.value })
                    }
                  />
                  <input
                    type="text"
                    value={editForm.surname}
                    onChange={(e) =>
                      setEditForm({ ...editForm, surname: e.target.value })
                    }
                  />
                  <input
                    type="text"
                    value={editForm.title}
                    onChange={(e) =>
                      setEditForm({ ...editForm, title: e.target.value })
                    }
                  />
                  <button onClick={() => handleUpdate(t.id)}>Kaydet</button>
                  <button onClick={() => setEditing(null)}>İptal</button>
                </>
              ) : (
                <>
                  <span>
                    {t.name} {t.surname} ({t.title})
                  </span>
                  <button onClick={() => handleEdit(t)}>Düzenle</button>
                </>
              )}
            </li>
          ))}
        </ul>
      </div>

      {/* Öğretmen Ekleme */}
      <div className="add-teacher">
        <h3>Add Teacher</h3>
        <input
          type="email"
          placeholder="Email"
          value={newTeacher.email}
          onChange={(e) =>
            setNewTeacher({ ...newTeacher, email: e.target.value })
          }
        />
        <input
          type="password"
          placeholder="Password"
          value={newTeacher.password}
          onChange={(e) =>
            setNewTeacher({ ...newTeacher, password: e.target.value })
          }
        />
        <input
          type="text"
          placeholder="Name"
          value={newTeacher.name}
          onChange={(e) =>
            setNewTeacher({ ...newTeacher, name: e.target.value })
          }
        />
        <input
          type="text"
          placeholder="Surname"
          value={newTeacher.surname}
          onChange={(e) =>
            setNewTeacher({ ...newTeacher, surname: e.target.value })
          }
        />
        <input
          type="text"
          placeholder="Title"
          value={newTeacher.title}
          onChange={(e) =>
            setNewTeacher({ ...newTeacher, title: e.target.value })
          }
        />
        <button onClick={handleCreate}>Create</button>
      </div>
    </div>
  );
};

export default Teachers;
