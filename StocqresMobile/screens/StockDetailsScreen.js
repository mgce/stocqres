import React, { Component } from "react";
import { StyleSheet, Modal } from "react-native";
import { Container, Content, Button, Text, View } from "native-base";
import { connect } from "react-redux";
import { padding, colors, fonts } from "../styles/common";
import _ from "lodash";
import { getStockDetails } from "../ducks/stocks";

const DetailsItem = props => (
  <View style={styles.labelContainer}>
    <Text style={styles.headerText}>{props.header}</Text>
    <Text style={styles.detailsText}>{props.details}</Text>
  </View>
);

class StockDetailsScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
      stock: {},
      addModalVisible: false
    };
  }
  async componentDidMount() {
    await this.props.getStockDetails(this.props.stockCode);
    const stock = await _.find(this.props.stockList, {
      Code: this.props.stockCode
    });
    await this.setState({ stock: stock });
  }
  toggleAddModal(visible) {
    this.setState({ addModalVisible: visible });
  }
  render() {
    return (
      <Container>
        <Content style={styles.content}>
          <Modal
            style={styles.modalContainer}
            animationType={"slide"}
            transparent={false}
            visible={this.state.addModalVisible}
            onRequestClose={() => {
              console.log("Modal closed");
            }}
          >
            <View>
              <Text>Modal</Text>
              <Button success onPress={()=>this.toggleAddModal(false)}>
              <Text>Close Modal</Text>
              </Button>
            </View>
          </Modal>
          <DetailsItem
            header={"Company Name"}
            details={this.state.stock.Name}
          />
          <DetailsItem
            header={"Company Code"}
            details={this.state.stock.Code}
          />
          <DetailsItem header={"Stock Unit"} details={this.state.stock.Unit} />
          <DetailsItem
            header={"Stock Price"}
            details={this.state.stock.Price}
          />
          <DetailsItem
            header={"Available Stocks"}
            details={this.props.stockDetails.stockQuantity}
          />
        </Content>
        <View style={styles.inlineButtons}>
          <Button
            success
            style={styles.button}
            onPress={()=>this.toggleAddModal(true)}>
            <Text style={styles.buttonText}>Buy</Text>
          </Button>
          <Button danger style={styles.button}>
            <Text style={styles.buttonText}>Sell</Text>
          </Button>
        </View>
      </Container>
    );
  }
}

const styles = StyleSheet.create({
  content: {
    flex: 1,
    paddingHorizontal: padding.md
  },
  headerText: {
    fontSize: fonts.sm,
    color: colors.darkSecondary
  },
  detailsText: {
    fontSize: fonts.md,
    color: colors.darkPrimary
  },
  inlineButtons: {
    // flex: 1,
    // flexGrow: 0,
    flexDirection: "row"
  },
  button: {
    borderRadius: 0,
    width: "50%",
    justifyContent: "center"
  },
  buttonText: {
    fontSize: fonts.md,
    alignSelf: "center"
  },
  modalContainer:{
    flex:1,
    marginHorizontal: padding.xl,
    height: '50%'
  }
});

const mapDispatchToProps = { getStockDetails };

const mapStateToProps = state => ({
  stockList: state.stocks.stockList,
  stockDetails: state.stocks.stockDetails
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(StockDetailsScreen);
