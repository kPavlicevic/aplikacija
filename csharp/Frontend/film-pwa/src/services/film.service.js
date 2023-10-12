import http from "../http-common";

class FilmDataService{

    async get(){
        return await http.get('/Film');
    }

    async getBySifra(sifra) {
        return await http.get('/film/' + sifra);
    }

    async delete(sifra){
        const odgovor = await http.delete('/Film/' + sifra)
        .then(response => {
            return {ok: true, poruka: 'Obrisao uspješno'};
        })
        .catch(e=>{
            return {ok: false, poruka: e.response.data};
        });

        return odgovor;
    }


    async post(film){
        const odgovor = await http.post('/film',film)
           .then(response => {
             return {ok:true, poruka: 'Unio film'}; // return u odgovor
           })
           .catch(error => {
            //console.log(error.response);
             return {ok:false, poruka: error.response.data}; // return u odgovor
           });
     
           return odgovor;
    }

    async put(sifra,film){
        //console.log(smjer);
        const odgovor = await http.put('/film/' + sifra,film)
           .then(response => {
             return {ok:true, poruka: 'Promjenio film'}; // return u odgovor
           })
           .catch(error => {
            //console.log(error.response);
             return {ok:false, poruka: error.response.data}; // return u odgovor
           });
     
           return odgovor;
         }


}

export default new FilmDataService();