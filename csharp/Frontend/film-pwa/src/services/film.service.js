import http from "../http-common";

class FilmDataService {
  async get() {
    return await http.get("/Film");
  }

  async getBySifra(sifra) {
    return await http.get("/film/" + sifra);
  }

  async getGlumci(sifra) {
    // console.log(sifra);
    return await http.get("/film/" + sifra + "/glumci");
  }

  async delete(sifra) {
    const odgovor = await http
      .delete("/Film/" + sifra)
      .then((response) => {
        return { ok: true, poruka: "Uspješno obrisano!" };
      })
      .catch((e) => {
        return { ok: false, poruka: e.response.data };
      });

    return odgovor;
  }

  async post(film) {
    const odgovor = await http
      .post("/film", film)
      .then((response) => {
        return { ok: true, poruka: "Film unešen!" }; // return u odgovor
      })
      .catch((error) => {
        //console.log(error.response);
        return { ok: false, poruka: error.response.data }; // return u odgovor
      });

    return odgovor;
  }

  async put(sifra, film) {
    //console.log(film);
    const odgovor = await http
      .put("/film/" + sifra, film)
      .then((response) => {
        return { ok: true, poruka: "Film promjenjen!" }; // return u odgovor
      })
      .catch((error) => {
        //console.log(error.response);
        return { ok: false, poruka: error.response.data }; // return u odgovor
      });

    return odgovor;
  }

  async obrisiGlumca(film, glumac){
    
    const odgovor = await http.delete('/Film/' + film + '/obrisiGlumca/' + glumac)
       .then(response => {
         return {ok:true, poruka: 'Uspješno obrisano!'};
       })
       .catch(error => {
         console.log(error);
         return {ok:false, poruka: error.response.data};
       });
 
       return odgovor;
     }
}

export default new FilmDataService();
