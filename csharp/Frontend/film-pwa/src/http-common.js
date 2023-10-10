import axios from "axios";

export default axios.create({
    baseURL: "https://kpavlicevic-001-site1.ctempurl.com/api/v1",
    headers: {
        "Content-Type": "application/json",
    }
});