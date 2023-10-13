import httpCommon from "../http-common";

class GlumacDataService{
 
    async get(){
        return httpCommon.get("Glumac");
    }
}

export default new GlumacDataService();