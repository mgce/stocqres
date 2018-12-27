import React, { Component } from "react";
import { connect } from "react-redux";
import { StyleSheet, View } from "react-native";
import {
  Container,
  Content,
  Text,
  Spinner,
  Button,
  Form,
  Label,
  Input,
  Item
} from "native-base";
import { goToAuth, goToHome } from "./navigation";
import { AsyncStorage } from "react-native";
import constants from "../common/constants";
import { fonts, colors, padding } from "../styles/common";
import { createWallet, getWalletDetails } from "../ducks/wallet";

class MyStocksScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
      creating: false,
      amount: 0
    };
  }
  componentDidMount() {
    this.props.getWalletDetails("00000000-0000-0000-0000-000000000000");
  }
  render() {
    var walletExist =
      this.props.walletId !== "00000000-0000-0000-0000-000000000000";
    var infoSection = (
      <View style={styles.createContainer}>
        <Text>Wallet Created</Text>
      </View>
    );
    var createSection = (
      <View>
        <View style={styles.textContainer}>
          <Text style={styles.header}>Create your wallet first</Text>
          <Text style={styles.paragraph}>
            Provide wallet amount and click "create"!
          </Text>
        </View>
        <Form>
          <Item floatingLabel>
            <Label>Wallet Amount</Label>
            <Input onChangeText={amount => this.setState({ amount })} />
          </Item>
          <Button
            success
            style={styles.button}
            onPress={() => {
              this.props.createWallet(this.state.amount);
            }}
          >
            <Text>Create</Text>
          </Button>
        </Form>
      </View>
    );
    return (
      <Container>
        <Content contentContainerStyle={styles.createContainer}>
          {walletExist ? infoSection : createSection}
        </Content>
      </Container>
    );
  }
}

const styles = StyleSheet.create({
  createContainer: {
    paddingHorizontal: padding.sm,
    paddingVertical: padding.md
  },
  header: {
    fontSize: fonts.lg,
    color: colors.darkPrimary
  },
  paragraph: {
    fontSize: fonts.md,
    color: colors.darkSecondary
  },
  textContainer: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center"
  },
  button: {
    width: "100%",
    alignSelf: "stretch",
    justifyContent: "center",
    paddingVertical: -padding.sm,
    marginTop: padding.md
  }
});

const mapDispatchToProps = { createWallet, getWalletDetails };

const mapStateToProps = state => ({
  walletId: state.authentication.walletId
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MyStocksScreen);
