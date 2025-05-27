import { showComponent, hideComponent } from './index.js';

const usernameText = document.getElementById('username-text');
const authButton = document.querySelector('.header__auth-button');

export const login = (username) => {
	if (!username) {
		return false;
	}

	localStorage.setItem('user', username);
	return true;
};

export const logout = () => {
	localStorage.removeItem('user');
};

export const getUser = () => {
	return localStorage.getItem('user');
};

export const checkAuth = () => {
	return !!getUser();
};

export const authConfiguration = () => {
	if (checkAuth()) {
		showComponent('.header__auth-text');
		usernameText.textContent = getUser();
		authButton.textContent = 'Log Out';
		showComponent('.promo__button');
	} else {
		hideComponent('.header__auth-text');
		authButton.textContent = 'Log In';
		hideComponent('.promo__button');
	}
};
