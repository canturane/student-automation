import api from "./api";

export const StudentService = {
  getAll: () => api.get("/Students"),   
  getOwn: () => api.get("/Students/me"),
  create: (data) => api.post("/Students", data),
  update: (id, data) => api.put(`/Students/${id}`, data),
};
