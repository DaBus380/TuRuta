import AuthenticationContext, { AuthenticationContextOptions } from "adal-angular";

export default class Authentication {
    config: AuthenticationContextOptions = {
        clientId: "7fb4b210-39d4-40bb-8989-c5981a1360f1",
        redirectUri: window.location.href,
        cacheLocation: "localStorage",
        postLogoutRedirectUri: window.location.origin,
        instance: "https://login.microsoftonline.com/"
    };

    authenticationContext? = new AuthenticationContext(this.config);

    initialize(){
        if(this.authenticationContext!.isCallback(window.location.hash)){
            this.authenticationContext!.handleWindowCallback();
        }

        else{
            let user = this.authenticationContext!.getCachedUser();
            if(user){
                return user;
            }

            this.signIn();
        }
    }

    acquireToken(): Promise<string> {
        return this.acquireTokenWrapper();
    }

    isAuthenticated(){
        if (this.authenticationContext!.getCachedToken(this.config.clientId)) { return true; }
        return false;
    }

    acquireTokenRedirect(){
        this.authenticationContext!.acquireTokenRedirect(this.config.clientId);
    }

    signOut(){
        this.authenticationContext!.logOut();
    }

    signIn(){
        this.authenticationContext!.login();
    }

    private acquireTokenWrapper(): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            this.authenticationContext!.acquireToken(this.config.clientId, (err, token) =>{
                if(err || !token){
                    return reject(err);
                }

                return resolve(token);
            })
        });
    }
}