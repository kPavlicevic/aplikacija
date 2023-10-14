import axios from "axios";

// "https://kpavlicevic-001-site1.ctempurl.com/api/v1"
export default axios.create({
    baseURL: "https://localhost:7008/api/v1",
    headers: {
        "Content-Type": "application/json",
    }
});

