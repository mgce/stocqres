import React, { Component } from "react";
import { FlatList } from "react-native";
import { Container, Content, Text } from "native-base";
import { connect } from "react-redux";
import { assignStockList } from "../../ducks/stocks";
import { default as StockListItem } from "./StockListItem";

export class MainScreen extends Component {
  constructor(props) {
    super(props);
    this.state = {
      connected: false
    };
  }
  componentDidMount() {
    //this.configureWebSockets();
  }
  configureWebSockets() {
    const ws = new WebSocket(
      "ws://webtask.future-processing.com:8068/ws/stocks"
    );
    ws.onopen = () => {
      this.setState({ connected: true });
    };
    ws.onmessage = e => {
      console.log("Message: ", e.data);
      this.props.assignStockList(e.data);
    };
    ws.onclose = () => {
      this.setState({ connected: false });
    };
  }
  _renderItem = ({ item }) => (
    <StockListItem
      id={item.Id}
      code={item.Code}
      name={item.Name}
      unit={item.Unit}
      price={item.Price}
      increased={item.Increased}
      percentageRate={item.PercentageRate}
      componentId={this.props.componentId}
    />
  );
  render() {
    return (
      <Container>
        <Content>
          <FlatList data={this.props.stockList} renderItem={this._renderItem} />
        </Content>
      </Container>
    );
  }
}

const mapDispatchToProps = {
  assignStockList
};

const mapStateToProps = state => ({
  stockList: state.stocks.stockList
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(MainScreen);
