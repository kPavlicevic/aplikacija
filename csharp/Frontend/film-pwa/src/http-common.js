import axios from "axios";

// 
export default axios.create({
    baseURL: "https://localhost:7008/api/v1",
    headers: {
        "Content-Type": "application/json",
    }
});

//"https://localhost:7008/api/v1"