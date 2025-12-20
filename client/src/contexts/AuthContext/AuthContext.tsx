import React, {createContext, useState, useEffect} from "react";
import { jwtDecode } from "jwt-decode";

interface CognitoJWT {
    sub: string;
    email: string;
    "cognito:username": string;
    exp: number;
}

interface AuthState {
    isAuthenticated: boolean;
    username: string | null;
    email: string | null;
    cognitoId: string | null;
    idToken: string | null;
    accessToken: string | null;
    logout: () => void;
    getAuthHeaders: () => Record<string, string>;
}

const AuthContext = createContext<AuthState | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode}> = ({children}) => {
    // Intialise AuthState while Omitting setAuthState value 
    const [authState, setAuthState] = useState<Omit<AuthState, "logout" | "getAuthHeaders">>({
        isAuthenticated: false,
        username: null,
        email: null,
        cognitoId: null,
        idToken: null,
        accessToken: null,
    })

    // Function to log user out and reset authState
    const logout = () => {
        // Clear tokens from localStorage
        localStorage.removeItem('idToken');
        localStorage.removeItem('accessToken');

        // Return State to Unauthenticated
        setAuthState({
            isAuthenticated: false,
            username: null,
            email: null,
            cognitoId: null,
            idToken: null,
            accessToken: null,
        });
        
        // Redirect to Cognito Logout
        const cognitoDomain = 'vynyl-app-gallie.auth.us-east-1.amazoncognito.com';
        const clientId = '6hpe4kcbkvf9hogee7kg0bo1h3';
        const logoutUri = 'http://localhost:5173';

        window.location.href = `https://${cognitoDomain}/logout?client_id=${clientId}&logout_uri=${logoutUri}`;
    };

    useEffect(() => {
        // Check for tokens in localStorage on mount
        const checkAuth = async () => {
            try {
                const idToken = localStorage.getItem('idToken');
                const accessToken = localStorage.getItem('accessToken');

                if(idToken && accessToken) {
                    // Decode the ID token to get user info
                    const decoded = jwtDecode<CognitoJWT>(idToken);

                    // Check if token is expired
                    const currentTime = Date.now() / 1000;
                    if(decoded.exp < currentTime) {
                        // Token expired, clear everything
                        logout();
                        return;
                    }

                    setAuthState({
                        isAuthenticated: true,
                        username: decoded["cognito:username"],
                        email: decoded.email,
                        cognitoId: decoded.sub,
                        idToken,
                        accessToken,
                    })
                } else {
                    // No tokens found
                    setAuthState({
                        isAuthenticated: false,
                        username: null,
                        email: null,
                        cognitoId: null,
                        idToken: null,
                        accessToken: null,
                    });
                }
            } catch (error) {
                console.error('Error checking authentication:', error);
                logout();
            }
        };

        checkAuth();
    }, []);

    // Helper function to get auth headers for API calls
    const getAuthHeaders = (): Record<string, string> => {
        if(authState.accessToken) {
            return {
                'Authorization': `Bearer ${authState.accessToken}`,
                'Content-Type': 'application/json',
            };
        }
        return {
            'Content-Type': 'application/json',
        };
    };

    return(
        <AuthContext.Provider value={{ ...authState, logout, getAuthHeaders }}>
            {children}
        </AuthContext.Provider>
    )
};

export { AuthContext };
export type { AuthState };
