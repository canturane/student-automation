import api from "./api";

export const TeacherService = {
  getAll: () => api.get("/Teachers"),
  create: (data) => api.post("/Teachers", data),
  update: (id, data) => api.put(`/Teachers/${id}`, data),
};
