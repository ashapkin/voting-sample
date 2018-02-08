import './css/site.css';
import 'bootstrap';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import Application from './components/Application'

function renderApp() {
	// This code starts up the React app when it runs in a browser. It sets up the routing
	// configuration and injects the app into a DOM element.
	ReactDOM.render(
		<Application />,
		document.getElementById('react-app')
	);
}

renderApp();