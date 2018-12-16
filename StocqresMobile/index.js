import { Navigation } from "react-native-navigation";
import { registerScreens } from "./screens/index";
import { Provider } from "react-redux";
import { default as store } from "./store";
import { persistStore, persistReducer } from "redux-persist";

Navigation.events().registerAppLaunchedListener(() => {
  persistStore(store, null, () => {
    registerScreens(store, Provider);
    Navigation.setRoot({
      root: {
        stack: {
          options: {
            topBar: {
              visible: false
            }
          },
          children: [
            {
              component: {
                name: "Initializing"
              }
            }
          ]
        }
      }
    });
  });
});
