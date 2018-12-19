import React, { Component } from "react";
import { StyleSheet } from "react-native";
import {
  Container,
  Content,
  Form,
  Item,
  Input,
  Label,
  Button,
  Text,
  Spinner
} from "native-base";
import { connect } from "react-redux";
import { Navigation } from "react-native-navigation";
import { login } from "../ducks/authentication";
import { goToHome } from "./navigation";
import PropTypes from "prop-types";

export class LoginScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
      username: "Adam",
      password: "Adamek123"
    };
    this.pushRegisterScreen = this.pushRegisterScreen.bind(this);
  }
  loginTo() {
    const command = this.state;
    this.props.login(command).then(() => {
      goToHome();
    });
  }
  pushRegisterScreen() {
    Navigation.push(this.props.componentId, {
      component: {
        name: "Register",
        options: {
          topBar: {
            visible: false
          }
        }
      }
    });
  }

  render() {
    return (
      <Container>
        <Content contentContainerStyle={styles.container}>
          {this.props.loading && <Spinner />}
          <Form style={styles.form}>
            <Item floatingLabel>
              <Label>Username</Label>
              <Input onChangeText={username => this.setState({ username })} />
            </Item>
            <Item floatingLabel>
              <Label>Password</Label>
              <Input onChangeText={password => this.setState({ password })} />
            </Item>
            <Button
              primary
              style={styles.submit}
              onPress={() => this.loginTo()}
            >
              <Text>Sign In</Text>
            </Button>
          </Form>
          <Text style={styles.registerText} onPress={this.pushRegisterScreen}>
            Doesn't have account? Create it!
          </Text>
        </Content>
      </Container>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingLeft: 15,
    paddingRight: 15,
    flexDirection: "column",
    alignItems: "center",
    justifyContent: "center"
  },
  submit: {
    alignSelf: "stretch",
    borderRadius: 10,
    justifyContent: "center",
    marginTop: 40
  },
  form: {
    alignSelf: "stretch",
    backgroundColor: "#ffffff",
    borderColor: "#d6d7da",
    borderRadius: 10,
    borderWidth: 1,
    padding: 10,
    paddingBottom: 30
  },
  registerText: {
    flexDirection: "column",
    fontSize: 12,
    justifyContent: "center",
    paddingTop: 10
  }
});

LoginScreen.propTypes = {
  login: PropTypes.func,
  componentId: PropTypes.string,
  loading: PropTypes.bool
};

const mapDispatchToProps = {
  login
};

const mapStateToProps = state => ({
  loading: state.authentication.loading
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LoginScreen);
