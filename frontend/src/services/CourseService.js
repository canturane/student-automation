import api from "./api";

export const CourseService = {
  getAll: () => api.get("/Courses"),                
  getMine: () => api.get("/Courses/mine"),          
  create: (data) => api.post("/Courses", data),     
  updateStatus: (courseId, data) =>
    api.put(`/Courses/${courseId}/status`, data),   
  addStudent: (courseId, studentId) =>
    api.post(`/Courses/${courseId}/students`, { studentId }),
  removeStudent: (courseId, studentId) =>
    api.delete(`/Courses/${courseId}/students/${studentId}`),
  getStudents: (courseId) => api.get(`/Courses/${courseId}/students`), 
};
