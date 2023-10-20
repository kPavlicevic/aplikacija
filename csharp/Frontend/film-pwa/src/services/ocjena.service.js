import http from "../http-common";

class OcjenaService {
  async post(sifra, ocjenaDto) {
    const odgovor = await http
      .post("/Ocjena/" + sifra + "/dodajOcjenu", ocjenaDto)
      .then((response) => {
        if (response && response.status === 200) {
          return { ok: true, prusmjeri: "/filmovi" };
        }
      })
      .catch((e) => {
        return { ok: false, error: e.response.data };
      });
    return odgovor;
  }

  async delete(sifraFilma, korisnickoIme) {
    const url =
      "/Ocjena/obrisi?sifraFilma=" + sifraFilma + "&korisnickoIme=" + korisnickoIme;
    const odgovor = await http
      .delete(url)
      .then((response) => {
        if (response && response.status === 200) {
          return { ok: true, preusmjeri: "/filmovi" };
        }
      })
      .catch((e) => {
        return { ok: false, error: e.response.data };
      });
    return odgovor;
  }

  async getProsjek(sifraFilma){
    return await http.get("Ocjena/"+ sifraFilma +"/ocjeneProsjek");
  }
}

export default new OcjenaService();
