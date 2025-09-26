/* @refresh reload */
import { render } from "solid-js/web";
import { Route, Router } from "@solidjs/router";
import { lazy } from "solid-js";
import "solid-devtools";
import "@picocss/pico";
import './index.css';

const root = document.getElementById("root");

if (import.meta.env.DEV && !(root instanceof HTMLElement)) {
  throw new Error(
    "Root element not found. Did you forget to add it to your index.html? Or maybe the id attribute got misspelled?"
  );
}

const Landing = lazy(() => import("./Landing"));

render(
  () => (
    <Router>
      <Route path="/" component={Landing}></Route>
    </Router>
  ),
  root!
);
