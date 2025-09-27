import * as React from 'react';
import {useState, useMemo, useEffect} from 'react';
import {createRoot} from 'react-dom/client';
import Map, {
  Marker,
  Popup,
  NavigationControl,
  FullscreenControl,
  ScaleControl,
  GeolocateControl
} from 'react-map-gl/mapbox';

import ControlPanel from './control-panel';
import Pin from './pin';
import {config, api} from './config';

import CITIES from '../cities.json';

type Nullable<T> = T | null | undefined;

interface PointDto {
  latitude: number;
  longitude: number;
}

interface TweetResponse {
  boxId: string;
  sasUri?: Nullable<string>;
  uploadedAt: string;
  isOccupied?: Nullable<boolean>;
  birdType?: Nullable<string>;
  eggCount?: Nullable<number>;
  hatchedCount?: Nullable<number>;
  deadCount?: Nullable<number>;
  description?: Nullable<string>;
}

interface BoxResponse {
  id: string;
  name?: Nullable<string>;
  point: PointDto;
  boxType: string;
  createdAt: string;
  isArchived: boolean;
  latestTweet?: Nullable<TweetResponse>;
}

const relativeTimeFormatter = new Intl.RelativeTimeFormat('de-DE', {numeric: 'auto'});

function formatDate(value?: Nullable<string>) {
  if (!value) {
    return '–';
  }

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return value;
  }

  return new Intl.DateTimeFormat('de-DE', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(date);
}

function formatRelative(value?: Nullable<string>) {
  if (!value) {
    return '–';
  }

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return value;
  }

  const elapsed = date.getTime() - Date.now();
  const minute = 60 * 1000;
  const hour = 60 * minute;
  const day = 24 * hour;
  const week = 7 * day;

  if (Math.abs(elapsed) < minute) {
    return 'eben';
  }

  if (Math.abs(elapsed) < hour) {
    return relativeTimeFormatter.format(Math.round(elapsed / minute), 'minute');
  }

  if (Math.abs(elapsed) < day) {
    return relativeTimeFormatter.format(Math.round(elapsed / hour), 'hour');
  }

  if (Math.abs(elapsed) < week) {
    return relativeTimeFormatter.format(Math.round(elapsed / day), 'day');
  }

  return relativeTimeFormatter.format(Math.round(elapsed / week), 'week');
}

function formatCoordinate(value?: Nullable<number>) {
  if (typeof value !== 'number' || Number.isNaN(value)) {
    return '–';
  }

  return value.toFixed(5);
}

function BoxPopupContent({box}: {box: BoxResponse}) {
  const displayName = box.name?.trim() || 'Unbenannte Box';
  const shortId = box.id?.split('-')[0]?.toUpperCase() ?? box.id;
  const statusClass = box.isArchived ? 'status-badge status-badge--archived' : 'status-badge status-badge--active';
  const statusLabel = box.isArchived ? 'Archiviert' : 'Aktiv';
  const latest = box.latestTweet ?? undefined;

  return (
    <div className="popup-card">
      <header className="popup-card__header">
        <div className="popup-card__headline">
          <h3 className="popup-card__title">{displayName}</h3>
          <p className="popup-card__subtitle">
            {box.boxType ?? 'Unbekannter Typ'} · #{shortId}
          </p>
        </div>
        <span className={statusClass}>{statusLabel}</span>
      </header>

      <section className="popup-card__section">
        <dl className="popup-card__details">
          <div>
            <dt>Erstellt</dt>
            <dd>{formatDate(box.createdAt)}</dd>
          </div>
          <div>
            <dt>Box-ID</dt>
            <dd className="popup-card__code">{box.id}</dd>
          </div>
        </dl>
      </section>

      <section className="popup-card__section popup-card__coords">
        <div>
          <span>Breite</span>
          <strong>{formatCoordinate(box.point?.latitude)}</strong>
        </div>
        <div>
          <span>Länge</span>
          <strong>{formatCoordinate(box.point?.longitude)}</strong>
        </div>
      </section>

      {latest ? (
        <section className="popup-card__section popup-card__section--tweet">
          <div className="popup-card__section-header">
            <h4>Letztes Update</h4>
            <span>{formatRelative(latest.uploadedAt)}</span>
          </div>
          {latest.description && (
            <p className="popup-card__description">{latest.description}</p>
          )}
          <ul className="popup-card__list">
            {typeof latest.isOccupied === 'boolean' && (
              <li>
                <strong>Status:</strong> {latest.isOccupied ? 'Belegt' : 'Frei'}
              </li>
            )}
            {latest.birdType && (
              <li>
                <strong>Vogelart:</strong> {latest.birdType}
              </li>
            )}
            {typeof latest.eggCount === 'number' && (
              <li>
                <strong>Eier:</strong> {latest.eggCount}
              </li>
            )}
            {typeof latest.hatchedCount === 'number' && (
              <li>
                <strong>Geschlüpft:</strong> {latest.hatchedCount}
              </li>
            )}
            {typeof latest.deadCount === 'number' && (
              <li>
                <strong>Verluste:</strong> {latest.deadCount}
              </li>
            )}
          </ul>
          {latest.sasUri && (
            <a
              className="popup-card__link"
              href={latest.sasUri}
              target="_blank"
              rel="noreferrer"
            >
              Foto ansehen
            </a>
          )}
        </section>
      ) : (
        <section className="popup-card__section popup-card__section--tweet popup-card__section--empty">
          <div className="popup-card__section-header">
            <h4>Letztes Update</h4>
            <span>Keine Daten</span>
          </div>
          <p className="popup-card__description">Für diese Box liegen noch keine Beobachtungen vor.</p>
        </section>
      )}
    </div>
  );
}

export default function App() {
  const [popupInfo, setPopupInfo] = useState<BoxResponse | null>(null);
  const [boxes, setBoxes] = useState<BoxResponse[]>([]);

  const handleFabClick = () => {
    // Hier können Sie die gewünschte Aktion hinzufügen
    console.log('FAB clicked!');
    // Beispiel: Zur aktuellen Position navigieren oder eine Aktion ausführen
  };

  useEffect(() => {
    const load = async () => {
      try {
        const response = await api.getBoxes(
          config.defaultLocation.latitude,
          config.defaultLocation.longitude
        );

        if (!response.ok) {
          throw new Error(`API responded with ${response.status}`);
        }

        const boxData: BoxResponse[] = await response.json();
        setBoxes(boxData);
      } catch (error) {
        console.error('Error loading boxes:', error);
        setBoxes([]);
      }
    };
    load();
  }, []);

  useEffect(() => {
    console.log('Boxes updated:', boxes);
  }, [boxes]);

  const pins = useMemo(
    () =>
      boxes
        .filter(box => box.point && typeof box.point.latitude === 'number' && typeof box.point.longitude === 'number')
        .map(box => (
          <Marker
            key={box.id}
            longitude={Number(box.point.longitude)}
            latitude={Number(box.point.latitude)}
            anchor="bottom"
            onClick={e => {
              e.originalEvent.stopPropagation();
              setPopupInfo(box);
            }}
          >
            <Pin />
          </Marker>
        )),
    [boxes]
  );

  return (
    <>
      <Map
        initialViewState={{
          latitude: config.defaultLocation.latitude,
          longitude: config.defaultLocation.longitude,
          zoom: config.defaultLocation.zoom,
          bearing: 0,
          pitch: 0
        }}
        mapStyle={config.mapbox.defaultStyle}
        mapboxAccessToken={config.mapbox.token}
      >
        <GeolocateControl position="top-left" />
        <FullscreenControl position="top-left" />
        <NavigationControl position="top-left" />
        <ScaleControl />

        {pins}

        {popupInfo && (
          <Popup
            className="box-popup"
            anchor="top"
            longitude={Number(popupInfo.point?.longitude)}
            latitude={Number(popupInfo.point?.latitude)}
            closeOnClick={false}
            onClose={() => setPopupInfo(null)}
          >
            <BoxPopupContent box={popupInfo} />
          </Popup>
        )}
      </Map>

      <ControlPanel />
      
      <button className="fab" onClick={handleFabClick} title="Neue Aktion">
        <svg viewBox="0 0 24 24">
          <path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z"/>
        </svg>
      </button>
    </>
  );
}

export function renderToDom(container) {
  createRoot(container).render(<App />);
}
