import {BACKEND_URL} from "../configuration/PasswordlessOptions";

export default class YourBackendClient {
    constructor() {

    }

    async register(user, firstName, lastName, deviceName) {
        const request = {
            username: user,
            firstName: firstName,
            lastName: lastName,
            deviceName: deviceName
        };
        const registerToken = await fetch(`${BACKEND_URL}/signup`,
            {
                method: 'post',
                body: JSON.stringify(request),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            }).then(r => r.json());
        return registerToken;
    }

    async signIn(token) {
        return await fetch(`${BACKEND_URL}/signin?token=${token}`).then(r => r.json());
    }
}