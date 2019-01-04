import axios from "axios";
import { AsyncStorage } from "react-native";
import constants from "../common/constants";
import { goToAuth } from "../screens/navigation";

const httpClient = axios.create({
  baseURL: "http://10.0.2.2:5001/api",
  responseType: "json",
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json"
  }
});

httpClient.interceptors.request.use(
  async function(config) {
    var token = await AsyncStorage.getItem(constants.ACCESS_TOKEN);

    if (token !== null) config.headers["Authorization"] = `Bearer ${token}`;

    return Promise.resolve(config);
  },
  function(error) {
    return Promise.reject(error);
  }
);

httpClient.interceptors.response.use(undefined, async error => {
  const originalRequest = error.config;
  const status = error.response.status;
  if (status !== 401) return Promise.reject(error);
  var accessToken = await getAccessToken();

  if (accessToken === "") return Promise.reject(error);
  originalRequest.headers["Authorization"] = `Bearer ${accessToken}`;
  return axios.request(originalRequest);
});

async function getAccessToken() {
  var refreshToken = await AsyncStorage.getItem(constants.REFRESH_TOKEN);
  var accessToken = "";

  axios
    .get(
      constants.BASE_ADDRESS + `/tokens/refresh-tokens/${refreshToken}/refresh`
    )
    .then(res => {
      AsyncStorage.setItem(constants.ACCESS_TOKEN, res.data.accessToken);
    })
    .catch(error =>{ 
      AsyncStorage.clear();
      goToAuth()
      });

  return accessToken;
}

export default httpClient;
