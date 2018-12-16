import React from "react";
import { connect } from "react-redux";
import { Container, Content, Text } from "native-base";
import { goToAuth, goToHome } from "./navigation";
import PropTypes from 'prop-types';

class Initialize extends React.PureComponent {
  async componentDidMount() {
    try {
      if (
        this.props.accessToken !== null &&
        this.props.accessToken !== undefined &&
        this.props.accessToken !== ""
      ) {
        goToHome();
      } else goToAuth();
    } catch (err) {
      goToAuth();
    }
  }

  render(){
    return (<Container>
      <Content>
        <Text>Loading</Text>
      </Content>
    </Container>);
  };
}

Initialize.propTypes = {
  accessToken: PropTypes.string
}

const mapStateToProps = state => ({
  accessToken: state.authentication.accessToken
});

export default connect(mapStateToProps)(Initialize);
