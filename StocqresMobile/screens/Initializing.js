import React from "react";
import { connect } from "react-redux";
import { Container, Content, Text, Spinner } from "native-base";
import { goToAuth, goToHome } from "./navigation";
import PropTypes from "prop-types";
import { AsyncStorage } from "react-native";
import constants from "../common/constants";

class Initialize extends React.PureComponent {
  async componentDidMount() {
    //sprawdzic czy token istnieje w asyncstorage
    const accessToken = await AsyncStorage.getItem(constants.ACCESS_TOKEN);
    const refreshToken = await AsyncStorage.getItem(constants.REFRESH_TOKEN);
    if(accessToken && refreshToken)
      goToHome();
    else
      goToAuth();
  }
  render() {
    return (
      <Container>
        <Content>
          <Spinner />
        </Content>
      </Container>
    )
  }
}

Initialize.propTypes = {
  accessToken: PropTypes.string
};

const mapStateToProps = state => ({
  accessToken: state.authentication.accessToken
});

export default connect(mapStateToProps)(Initialize);
