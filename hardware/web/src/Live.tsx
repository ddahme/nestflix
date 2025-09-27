import { createSignal, Show } from "solid-js";
import "./Live.css";

const BASE_URL =
  "https://ca-api.livelymeadow-be846269.germanywestcentral.azurecontainerapps.io/";

const sendTweet = async (tweet: {
  boxId: string;
  imageBase64: string;
  isOccupied: boolean;
  birdType: string;
  eggCount: number;
  hatchedCount: number;
  deadCount: number;
  description: string;
}) => {
  // await fetch(`${BASE_URL}api/boxes/${tweet.boxId}/tweets`, {
  //   method: "POST",
  //   body: JSON.stringify(tweet),
  // });
  await new Promise((resolve) => window.setTimeout(resolve, 4_000));
};

const Live = () => {
  const [formState, setFormState] = createSignal<"unsent" | "waiting" | "sent">(
    "unsent"
  );
  const [isOccupied, setIsOccupied] = createSignal(false);
  const [birdType, setBirdType] = createSignal("");
  const [eggCount, setEggCount] = createSignal(0);
  const [hatchedCount, setHatchedCount] = createSignal(0);
  const [deadCount, setDeadCount] = createSignal(0);
  const [description, setDescription] = createSignal("");

  let imgRef: HTMLImageElement | undefined;

  const handleSubmitClick = async (ev: MouseEvent) => {
    ev.preventDefault();
    setFormState("waiting");
    try {
      let imgBase64 = "";
      if (imgRef) {
        const canvas = document.createElement("canvas");
        const ctx = canvas.getContext("2d");
        canvas.width = imgRef.naturalWidth;
        canvas.height = imgRef.naturalHeight;
        try {
          ctx!.drawImage(imgRef, 0, 0);
          imgBase64 = canvas.toDataURL("image/png");
        } catch {}
      }

      await sendTweet({
        boxId: "01998a50-83b5-7ab1-9a7d-427310422c91",
        imageBase64: imgBase64,
        isOccupied: isOccupied(),
        birdType: birdType(),
        eggCount: eggCount(),
        hatchedCount: hatchedCount(),
        deadCount: deadCount(),
        description: description(),
      });
      setFormState("sent");
    } catch (e) {
      console.error(e);
      setFormState("sent");
    }
  };

  return (
    <main class="container" style="padding-inline: 0;">
      <section>
        <img
          src="/birdie2.jpeg"
          alt="Livestream aus dem Nistkasten"
          ref={imgRef}
        />
      </section>
      <section>
        <Show when={formState() === "unsent"}>
          <h2>Was passiert gerade?</h2>
          <form>
            <fieldset>
              <legend>Ist jemand Zuhause?</legend>
              <label>
                <input
                  type="checkbox"
                  name="isOccupied"
                  onChange={(ev) => setIsOccupied(ev.target.value === "true")}
                />
                Das Nest ist bewohnt
              </label>
            </fieldset>

            <fieldset>
              <select
                name="birdType"
                aria-label="Welcher Vogel lebt hier?"
                onChange={(ev) => setBirdType(ev.target.value)}
              >
                <option selected disabled value="">
                  Welcher Vogel lebt hier?
                </option>
                <option>Amsel</option>
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
                  onChange={(ev) =>
                    setEggCount(Number.parseInt(ev.target.value))
                  }
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
                  onChange={(ev) =>
                    setHatchedCount(Number.parseInt(ev.target.value))
                  }
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
                Wie viele davon sind nicht wohlauf?
                <br />
                <input
                  type="range"
                  min="0"
                  max="10"
                  step="1"
                  list="aliveMarkers"
                  value="0"
                  onChange={(ev) =>
                    setDeadCount(Number.parseInt(ev.target.value))
                  }
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
                onChange={(ev) => setDescription(ev.target.value)}
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

        <Show when={formState() === "waiting"}>
          <h1>⬆️</h1>
          <h2>Danke für deine Hilfe!</h2>
          <small>
            Sobald du die Verbindung zum Nistkasten trennst, werden die Daten
            zur openBirdMap hochgeladen. Forschende und Interessierte aus aller
            Welt können diese Daten dann nutzen, um Maßnahmen zur Stärkung der
            Artenvielfalt zu verfolgen und zu verbessern.
          </small>
        </Show>

        <Show when={formState() === "sent"}>
          <h1>✅</h1>
          <h2>Danke für deine Hilfe!</h2>
          <small>
            Dein Beitrag wurde zur openBirdMap übertragen und ist nun online
            verfügbar.
          </small>
        </Show>
      </section>
    </main>
  );
};

export default Live;
