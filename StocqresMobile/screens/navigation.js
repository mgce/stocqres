import { Navigation } from "react-native-navigation";
import { colors } from "../styles/common";

export const goToAuth = () =>
  Navigation.setRoot({
    root: {
      stack: {
        id: "Authorization",
        options: {
          topBar: {
            visible: false
          }
        },
        children: [
          {
            component: {
              name: "Login"
            }
          }
        ]
      }
    }
  });

export const goToHome = () =>
  Navigation.setRoot({
    root: {
      stack: {
        id: "Main",
        options: {
          topBar: {
            visible: true,
            title: {
              text: "Stock Exchange",
              alignment: "center",
              color: colors.lightPrimary
            },
            background: {
              color: colors.darkPrimary
            }
          }
        },
        children: [
          {
            component: {
              name: "Main"
            }
          }
        ]
      }
    }
  });

export const goToStockDetails = (componentId, companyName, stockCode) =>
  Navigation.push(componentId, {
    component: {
      name: "StockDetails",
      passProps: {
        stockCode: stockCode,
        companyName: companyName
      },
      options: {
        topBar: {
          title: {
            text: companyName + " Details"
          },
          backButton:{
              color: colors.lightPrimary
          }
        }
      }
    }
  });
