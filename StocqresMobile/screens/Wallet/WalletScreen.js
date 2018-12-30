import React, { Component } from "react";
import { connect } from "react-redux";
import { StyleSheet, View } from "react-native";
import { Container, Content, Text, Spinner } from "native-base";
import { fonts, colors, padding } from "../../styles/common";
import { createWallet, getWalletDetails } from "../../ducks/wallet";
import CreateWalletSection from "./CreateWalletSection";
import WalletDetailsSection from "./WalletDetailsSection";

class WalletScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
      creating: false,
      amount: 0
    };
  }
  componentDidMount() {
    this.props.getWalletDetails();
  }
  render() {
    var walletExist = this.props.walletId !== {};
    var investorDetails = {
      firstName: this.props.firstName,
      lastName: this.props.lastName,
      amount: this.props.wallet.amount,
      stockList: this.props.wallet.stockList,
    };
    return (
      <Container>
        <Content contentContainerStyle={styles.createContainer}>
          {walletExist ? (
            <WalletDetailsSection investor={investorDetails} />
          ) : (
            <CreateWalletSection
              createWallet={this.props.createWallet}
              onAmountChange={amount => this.setState({ amount })}
            />
          )}
        </Content>
      </Container>
    );
  }
}

const styles = StyleSheet.create({
  createContainer: {
    // paddingHorizontal: padding.sm,
    // paddingVertical: padding.md
  }
});

const mapDispatchToProps = {
  createWallet,
  getWalletDetails
};

const mapStateToProps = state => ({
  wallet: state.wallet.wallet,
  firstName: state.investor.firstName,
  lastName: state.investor.lastName
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(WalletScreen);
