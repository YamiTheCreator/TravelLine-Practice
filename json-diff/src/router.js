import { showComponent, hideComponent } from './index.js';
import { login, logout, checkAuth, authConfiguration } from './auth.js';

const loginInput = document.getElementById('form__input');
const startButton = document.querySelector('.promo__button');
const logo = document.querySelector('.header__logo');
const authButton = document.querySelector('.header__auth-button');
const formButton = document.querySelector('.form__button');

const showPage = (page) => {
	hideComponent('.main__promo');
	hideComponent('.main__sign');
	hideComponent('.main__content');
	showComponent(page);
};

const toPromoPage = () => {
	logo.addEventListener('click', () => {
		showPage('.main__promo');
		authConfiguration();
	});

	authButton.addEventListener('click', () => {
		if (checkAuth()) {
			logout();
			authConfiguration();
			showPage('.main__promo');
		} else {
			showPage('.main__sign');
		}
	});

	startButton.addEventListener('click', () => {
		if (checkAuth()) {
			showPage('.main__content');
		} else {
			showPage('.main__sign');
		}
	});
};

const toAuthPage = () => {
	formButton.addEventListener('click', (e) => {
		e.preventDefault();
		const username = loginInput.value.trim();
		const inputElement = document.getElementById('form__input');

		if (!username) {
			inputElement.classList.add('form__input_error');
			showComponent('.form__error');
			return;
		}

		inputElement.classList.remove('form__input_error');
		hideComponent('.form__error');

		if (login(username)) {
			authConfiguration();
			showPage('.main__promo');
		}
	});

	loginInput.addEventListener('input', () => {
		if (loginInput.value.trim()) {
			loginInput.classList.remove('form__input_error');
			hideComponent('.form__error');
		}
	});
};

const initApp = () => {
	authConfiguration();
	showPage('.main__promo');
	toPromoPage();
	toAuthPage();
};

initApp();
