import { Navigation } from "react-native-navigation";

export function registerScreens(store, Provider) {
  Navigation.registerComponentWithRedux(
    "Login",
    () => require("./LoginScreen").default,
    Provider,
    store
  );
  Navigation.registerComponentWithRedux(
    "Main",
    () => require("./MainScreen").default,
    Provider,
    store
  );
  Navigation.registerComponentWithRedux(
    "Initializing",
    () => require("./Initializing").default,
    Provider,
    store
  );
  Navigation.registerComponentWithRedux(
    "Register",
    () => require("./RegisterScreen").default,
    Provider,
    store
  );
}
