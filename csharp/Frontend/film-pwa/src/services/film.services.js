import http from "../http-common";

class FilmDataService{

    async get(){
        return await http.get('/Film');
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


}

export default new FilmDataService();