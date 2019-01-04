import React, { Component } from "react";
import { connect } from "react-redux";
import { StyleSheet, View } from "react-native";
import { Container, Content, Text, Spinner } from "native-base";
import { fonts, colors, padding } from "../../styles/common";
import { createWallet, getWalletDetails } from "../../ducks/wallet";
import { getInvestorDetails } from "../../ducks/investor";
import CreateWalletSection from "./CreateWalletSection";
import WalletDetailsSection from "./WalletDetailsSection";

class WalletScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
      creating: false,
      amount: 0
    };
    this.onClickCreate = this.onClickCreate.bind(this);
  }
  componentDidMount() {
    this.props.getWalletDetails();
  }
  onClickCreate(){
    this.props.createWallet(this.state.amount);
    this.props.getInvestorDetails();
  }
  render() {
    var walletExist = this.props.walletId !== "00000000-0000-0000-0000-000000000000";
    var investorDetails = {
      firstName: this.props.firstName,
      lastName: this.props.lastName,
      amount: this.props.amount,
      stockList: this.props.stockList,
    };
    return (
      <Container>
        <Content contentContainerStyle={styles.createContainer}>
          {walletExist ? (
            <WalletDetailsSection investor={investorDetails} />
          ) : (
            <CreateWalletSection
              createWallet={this.onClickCreate}
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
  getWalletDetails,
  getInvestorDetails
};

const mapStateToProps = state => ({
  amount: state.wallet.amount,
  stockList: state.wallet.stockList,
  firstName: state.investor.firstName,
  lastName: state.investor.lastName,
  walletId: state.investor.walletId
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(WalletScreen);
