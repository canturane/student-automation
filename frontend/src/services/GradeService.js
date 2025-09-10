import api from "./api";

export const GradeService = {
  add: (data) => api.post("/Grades", data),   
  getMine: () => api.get("/Grades/mine"),     
};
