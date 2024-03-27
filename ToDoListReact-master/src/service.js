import axios from 'axios';
//הוספת מיירט ללכידת שגיעות
axios.interceptors.request.use(
  (config) =>
  config,
  (error) => {
    console.log(error);
    return Promise.reject(error);
  }
);
axios.interceptors.response.use(
  (response) =>
   response,
  (error) => {
    console.log(error);
    return Promise.reject(error);
  }
);

// axios.defaults.baseURL='http://localhost:5101/'
axios.defaults.baseURL=process.env.REACT_APP_API
const apiUrl=process.env.REACT_APP_API
export default {
  
  getTasks: async () => {   
    const result = await axios.get(`${apiUrl}`)    
    return result.data;
  },

  addTask: async(nmae)=>{
    const result=await axios.post(`Item/${nmae}`)
    return result.data;
  },
  
  setCompleted: async(id, IsComplete)=>{
    await axios.put(`Item/${id}/${IsComplete}`)
    return {};
  },

  deleteTask:async(id)=>{
    const result=await axios.delete(`Item/${id}`)
    return result.data;
  }
};
