import { useEffect, useState } from "react";
import { StudentService } from "../../services/StudentService";
import "../../styles/Students.css";

const Students = () => {
  const [students, setStudents] = useState([]);
  const [newStudent, setNewStudent] = useState({
    email: "",
    password: "",
    name: "",
    surname: "",
    number: "",
  });
  const [editing, setEditing] = useState(null);
  const [editForm, setEditForm] = useState({
    name: "",
    surname: "",
    number: "",
  });

  const loadStudents = async () => {
    const res = await StudentService.getAll();
    setStudents(res.data);
  };

  const handleCreate = async () => {
    await StudentService.create(newStudent);
    setNewStudent({ email: "", password: "", name: "", surname: "", number: "" });
    loadStudents();
  };

  const handleEdit = (student) => {
    setEditing(student.id);
    setEditForm({
      name: student.name,
      surname: student.surname,
      number: student.number,
    });
  };

  const handleUpdate = async (id) => {
    await StudentService.update(id, editForm);
    setEditing(null);
    loadStudents();
  };

  useEffect(() => {
    loadStudents();
  }, []);

  return (
    <div className="students-page">
      <h2>Students</h2>

      {/* Öğrenci Listesi */}
      <div className="students-list">
        <ul>
          {students.map((s) => (
            <li key={s.id}>
              {editing === s.id ? (
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
                    value={editForm.number}
                    onChange={(e) =>
                      setEditForm({ ...editForm, number: e.target.value })
                    }
                  />
                  <button onClick={() => handleUpdate(s.id)}>Kaydet</button>
                  <button onClick={() => setEditing(null)}>İptal</button>
                </>
              ) : (
                <>
                  <span>
                    {s.name} {s.surname} ({s.number})
                  </span>
                  <button onClick={() => handleEdit(s)}>Düzenle</button>
                </>
              )}
            </li>
          ))}
        </ul>
      </div>

      {/* Öğrenci Ekleme */}
      <div className="add-student">
        <h3>Add Student</h3>
        <input
          type="email"
          placeholder="Email"
          value={newStudent.email}
          onChange={(e) => setNewStudent({ ...newStudent, email: e.target.value })}
        />
        <input
          type="password"
          placeholder="Password"
          value={newStudent.password}
          onChange={(e) =>
            setNewStudent({ ...newStudent, password: e.target.value })
          }
        />
        <input
          type="text"
          placeholder="Name"
          value={newStudent.name}
          onChange={(e) => setNewStudent({ ...newStudent, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Surname"
          value={newStudent.surname}
          onChange={(e) =>
            setNewStudent({ ...newStudent, surname: e.target.value })
          }
        />
        <input
          type="text"
          placeholder="Number"
          value={newStudent.number}
          onChange={(e) =>
            setNewStudent({ ...newStudent, number: e.target.value })
          }
        />
        <button onClick={handleCreate}>Create</button>
      </div>
    </div>
  );
};

export default Students;
