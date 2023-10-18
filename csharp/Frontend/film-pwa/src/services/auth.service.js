import http from "../http-common";

class AuthService {
  async prijava(korisnikDto) {
    const odgovor = await http
      .post("/Korisnik/prijava", korisnikDto)
      .then((response) => {
        if (response.data) {
          const auth = {
            prijavljen: true,
            korisnickoIme: korisnikDto.korisnickoIme,
          };
          localStorage.setItem("auth", JSON.stringify(auth));
          return { ok: true, preusmjeri: "/" };
        } else {
          return { ok: false, error: "Lozinka je neispravna!" };
        }
      })
      .catch((e) => {
        return { ok: false, error: e.response.data };
      });

      return odgovor;
  }
}

export default new AuthService();
