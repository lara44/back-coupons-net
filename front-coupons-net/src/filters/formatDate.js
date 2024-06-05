export default {
    formatDate(value) {
      if (!value) return ""; 
      
      if (!(value instanceof Date)) {
        value = new Date(value);
      }
  
      const day = value.getDate().toString().padStart(2, "0");
      const month = (value.getMonth() + 1).toString().padStart(2, "0"); // Months are 0-indexed
      const year = value.getFullYear();
      return `${day}/${month}/${year}`;
    },
  };