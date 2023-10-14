import http from "../http-common";

class GlumacDataService {
  async getAll() {
    return await http.get("/glumac");
  }

  async getSlikeGlumaca() {
    return await http.get("/Slika/glumci");
  }

  async delete(sifra) {
    const odgovor = await http
      .delete("/glumac/" + sifra)
      .then((response) => {
        return { ok: true, poruka: "Obrisao uspješno" };
      })
      .catch((error) => {
        console.log(error);
        return { ok: false, poruka: error.response.data };
      });

    return odgovor;
  }
}

export default new GlumacDataService();
