import http from "../http-common";

class SlikaService {
  async getSlikeGlumaca() {
    return await http.get("/Slika/glumci");
  }

  async getSlikuGlumcaPoSifri(sifra) {
    const url = "/Slika?sifra=" + sifra + "&vrsta=2";
    const odgovor = await http
      .get(url)
      .then((response) => {
        return { ok: true, slika: response.data.bitovi };
      })
      .catch((e) => {
        return { ok: false, poruka: e.response.data };
      });
    return odgovor;
  }

  async getSlikuFilmaPoSifri(sifra) {
    const url = "Slika?sifra=" + sifra + "&vrsta=1";
    const odgovor = await http
      .get(url)
      .then((response) => {
        return { ok: true, slika: response.data.bitovi };
      })
      .catch((e) => {
        return { ok: false, poruka: e.response.data };
      });
    return odgovor;
  }

  async postaviSliku(formData) {
    const config = {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    };
    const odgovor = await http
      .post("/Slika", formData, config)
      .then((response) => {
        console.log(response);
        return { ok: true, poruka: "Slika postavljena!" };
      })
      .catch((error) => {
        console.log(error);
        return { ok: false, poruka: error.response.data };
      });

    return odgovor;
  }
}

export default new SlikaService();
