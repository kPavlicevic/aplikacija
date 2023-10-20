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
}

export default new KomentarService();
