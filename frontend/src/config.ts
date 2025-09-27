// Anwendungskonfiguration
export const config = {
	// API-Konfiguration
	api: {
		baseUrl:
			"https://ca-api.livelymeadow-be846269.germanywestcentral.azurecontainerapps.io",
		timeout: 10000, // 10 Sekunden
	},

	// Mapbox-Konfiguration
	mapbox: {
		token:
			"pk.eyJ1IjoiY3JpcGFjeCIsImEiOiJjbWcyMnM4cmswbWI2MmxzNjlmOTc3bnBnIn0.TtaNAfuhNge8o6IQYb7ZFg",
		defaultStyle: "mapbox://styles/mapbox/dark-v9",
	},

	// Standard-Koordinaten
	defaultLocation: {
		latitude: 51.9521821,
		longitude: 7.640492213940821,
		zoom: 14,
	},
};

// API-Utility-Funktionen
export const api = {
	// Spezifische API-Methoden
	async getBoxes(
		latitude: number,
		longitude: number,
		distanceInMeters: number = 50000,
		page: number = 1,
		size: number = 1000
	) {
		return fetch(
			`${config.api.baseUrl}/api/boxes?Latitude=${latitude}&Longitude=${longitude}&Page=${page}&Size=${size}&distanceInMeters=${distanceInMeters}`
		);
	},

	async addBox(boxData: any) {
		return fetch(`${config.api.baseUrl}/api/boxes`, {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			body: JSON.stringify(boxData),
		});
	},

	async getTweets(boxId: string, page: number = 1, size: number = 10) {
		return fetch(
			`${config.api.baseUrl}/api/boxes/${boxId}/tweets?Page=${page}&Size=${size}`
		);
	},
};
