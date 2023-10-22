import http from "../http-common";

class GlumacDataService {
  async getAll() {
    return await http.get("/Glumac");
  }

  async getBySifra(sifra) {
    return await http.get("/glumac/" + sifra);
  }

  async delete(sifra) {
    const odgovor = await http
      .delete("/Glumac" + sifra)
      .then((response) => {
        return { ok: true, poruka: "Uspješno obrisano!" };
      })
      .catch((error) => {
        console.log(error);
        return { ok: false, poruka: error.response.data };
      });

    return odgovor;
  }

  async post(glumac) {
    const odgovor = await http
      .post("/glumac", glumac)
      .then((response) => {
        return { ok: true, poruka: "Glumac unešen!" }; // return u odgovor
      })
      .catch((error) => {
        //console.log(error.response);
        return { ok: false, poruka: error.response.data }; // return u odgovor
      });

    return odgovor;
  }

  async put(sifra, glumac) {
    const odgovor = await http
      .put("/glumac/" + sifra, glumac)
      .then((response) => {
        return { ok: true, poruka: "Promjenio glumca" }; // return u odgovor
      })
      .catch((error) => {
        console.log(error.response);
        return { ok: false, poruka: error.response.data }; // return u odgovor
      });

    return odgovor;
  }

  
}

export default new GlumacDataService();
