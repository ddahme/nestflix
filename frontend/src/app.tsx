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

export default function App() {
  const [popupInfo, setPopupInfo] = useState(null);
  const [boxes, setBoxes] = useState([]);

  const handleFabClick = () => {
    // Hier können Sie die gewünschte Aktion hinzufügen
    console.log('FAB clicked!');
    // Beispiel: Zur aktuellen Position navigieren oder eine Aktion ausführen
  };

  useEffect(() => {
    const load = async () => {
      try {
        const data = await api.getBoxes(
          config.defaultLocation.latitude,
          config.defaultLocation.longitude
        );
        setBoxes(await data.json());
      } catch (error) {
        console.error('Error loading boxes:', error);
        setBoxes([]);
      }
    }
    load();
  }, []);

  useEffect(() => {
    console.log('Boxes updated:', boxes);
  }, [boxes]);

  const pins = 
      boxes.map((box, index) => (
        <Marker
          key={`marker-${index}`}
          longitude={box.point.longitude}
          latitude={box.point.latitude}
          anchor="bottom"
          onClick={e => {
            // If we let the click event propagates to the map, it will immediately close the popup
            // with `closeOnClick: true`
            e.originalEvent.stopPropagation();
            setPopupInfo(box);
          }}
        >
          <Pin />
        </Marker>
      ));

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
            anchor="top"
            longitude={Number(popupInfo.point.longitude)}
            latitude={Number(popupInfo.point.latitude)}
            onClose={() => setPopupInfo(null)}
          >
            <div>
              {JSON.stringify(popupInfo)} |{' '}
            </div>
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
