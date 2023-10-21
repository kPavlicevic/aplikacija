import http from "../http-common";

class KomentarService {
  async postKomentar(sifraFilma, komentarDto) {
    const odgovor = await http
      .post("/Komentar/" + sifraFilma + "/dodajKomentar", komentarDto)
      .then((response) => {
        return { ok: true };
      })
      .catch((e) => {
        return { ok: false, error: e.response.data };
      });
    return odgovor;
  }

  async delete(komentarSifra, korisnickoIme) {
    const url =
      "/Komentar/" +
      komentarSifra +
      "/obrisiKomentar?" +
      "korisnickoIme=" +
      korisnickoIme;
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

  async put(komentarDto) {
    const url = "/Komentar/" + komentarDto.Sifra;
    const odgovor = await http
      .put(url, komentarDto)
      .then((response) => {
        if (response && response.status === 200) {
          return { ok: true };
        }
      })
      .catch((e) => {
        return { ok: false, error: e.response.data };
      });
    return odgovor;
  }
}

export default new KomentarService();
