import { createSignal, Show } from "solid-js";
import "./Live.css";

const Live = () => {
  const [formSent, setFormSent] = createSignal(false);

  const handleSubmitClick = (ev: MouseEvent) => {
    ev.preventDefault();

    setFormSent(true);
  };

  return (
    <main class="container" style="padding-inline: 0;">
      <section>
        <img src="/stream" alt="Livestream aus dem Nistkasten" />
      </section>
      <section>
        <Show when={!formSent()}>
          <h2>Was passiert gerade?</h2>
          <form>
            <fieldset>
              <legend>Ist jemand Zuhause?</legend>
              <label>
                <input type="checkbox" name="isOccupied" />
                Das Nest ist bewohnt
              </label>
            </fieldset>

            <fieldset>
              <select name="birdType" aria-label="Welcher Vogel lebt hier?">
                <option selected disabled value="">
                  Welcher Vogel lebt hier?
                </option>
                <option>Bachstelze</option>
                <option>Blaumeise</option>
                <option>Buchfink</option>
                <option>Feldsperling</option>
                <option>Fitis</option>
                <option>Gartenbaumläufer</option>
                <option>Gartengrasmücke</option>
                <option>Gartenrotschwanz</option>
                <option>Gimpel</option>
              </select>
            </fieldset>

            <fieldset>
              <label>
                Sind Eier im Nest?
                <br />
                <input
                  type="range"
                  min="0"
                  max="10"
                  step="1"
                  list="eggCountMarkers"
                  value="0"
                />
              </label>
              <datalist id="eggCountMarkers">
                <option value="0" label="0"></option>
                <option value="1" label="1"></option>
                <option value="2" label="2"></option>
                <option value="3" label="3"></option>
                <option value="4" label="4"></option>
                <option value="5" label="5"></option>
                <option value="6" label="6"></option>
                <option value="7" label="7"></option>
                <option value="8" label="8"></option>
                <option value="9" label="9"></option>
                <option value="10" label="10"></option>
              </datalist>

              <label>
                Wie viele davon sind geschlüpft?
                <br />
                <input
                  type="range"
                  min="0"
                  max="10"
                  step="1"
                  list="hatchedMarkers"
                  value="0"
                />
              </label>
              <datalist id="hatchedMarkers">
                <option value="0" label="0"></option>
                <option value="1" label="1"></option>
                <option value="2" label="2"></option>
                <option value="3" label="3"></option>
                <option value="4" label="4"></option>
                <option value="5" label="5"></option>
                <option value="6" label="6"></option>
                <option value="7" label="7"></option>
                <option value="8" label="8"></option>
                <option value="9" label="9"></option>
                <option value="10" label="10"></option>
              </datalist>

              <label>
                Wie viele davon sind wohlauf?
                <br />
                <input
                  type="range"
                  min="0"
                  max="10"
                  step="1"
                  list="aliveMarkers"
                  value="0"
                />
              </label>
              <datalist id="aliveMarkers">
                <option value="0" label="0"></option>
                <option value="1" label="1"></option>
                <option value="2" label="2"></option>
                <option value="3" label="3"></option>
                <option value="4" label="4"></option>
                <option value="5" label="5"></option>
                <option value="6" label="6"></option>
                <option value="7" label="7"></option>
                <option value="8" label="8"></option>
                <option value="9" label="9"></option>
                <option value="10" label="10"></option>
              </datalist>
            </fieldset>
            <fieldset>
              <textarea
                name="description"
                placeholder="Gibt es sonst noch etwas Interessantes?"
                aria-label="Description for further information"
              ></textarea>
            </fieldset>
            <button type="submit" onclick={handleSubmitClick}>
              Tweet abschicken
            </button>
            <small>
              Wenn du das Formular abschickst, wird ein Foto aus dem Nistkasten
              zusammen mit den von dir angegebenen Daten zur openBirdMap
              geschickt und der Öffentlichkeit verfügbar gemacht. Vielen Dank
              für deine Mitarbeit!
            </small>
          </form>
        </Show>

        <Show when={formSent()}>
          <h1>✅</h1>
          <h2>Danke für deine Hilfe!</h2>
          <small>
            Sobald du die Verbindung zum Nistkasten trennst, werden die Daten
            zur openBirdMap hochgeladen. Forschende und Interessierte aus aller
            Welt können diese Daten dann nutzen, um Maßnahmen zur Stärkung der
            Artenvielfalt zu verfolgen und zu verbessern.
          </small>
        </Show>
      </section>
    </main>
  );
};

export default Live;
