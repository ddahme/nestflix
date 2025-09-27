import "./Landing.css"

const Landing = () => {
  return (
    <main class="container">
      <img src="/nestflix_logo.png" />
      <h1>Nestflix</h1>
      <p>Gleich gehts los.</p>

      <p>
        Damit Nestflix funktioniert, brauchen wir deine Einverständnis.
        Die Daten, die die Plattform am Leben erhalten, werden von Nutzenden wie dir hochgeladen, da die Nistkästen selbst über keine Internetverbindung verfügen.
        Dabei kann es zu erhöhtem Datenverbrauch kommen.

        Danke für deine Mithilfe!
      </p>

      <form class="pure-form" action="live">
        <label for="opt-in">
            <input id="opt-in" type="checkbox" required/> Nestflix darf meine Datenverbindung nutzen, um Updates über den Nistkasten hochzuladen.
        </label>
        <button type="submit" class="pure-button pure-button-primary">Zum Livestream</button>
      </form>
    </main>
  );
};

export default Landing;
